(() => {
    if (window.playrScrollNavInitialized) return;
    window.playrScrollNavInitialized = true;

    const MAX_SECTIONS = 9;
    const PIXELS_PER_SECTION = 900;

    let nav = null;
    let sectionCount = 0;
    let activeSection = -1;
    let resizeObserver = null;
    let observedElement = null;
    let scheduled = false;

    function getMaxScroll() {
        return Math.max(
            0,
            document.documentElement.scrollHeight - window.innerHeight
        );
    }

    function getSectionCount() {
        const maxScroll = getMaxScroll();

        if (maxScroll < 100) return 0;

        return Math.min(
            MAX_SECTIONS,
            Math.max(2, Math.ceil(maxScroll / PIXELS_PER_SECTION) + 1)
        );
    }

    function getActiveSection() {
        const maxScroll = getMaxScroll();

        if (maxScroll <= 0 || sectionCount <= 1) return 0;

        const progress = Math.min(
            1,
            Math.max(0, window.scrollY / maxScroll)
        );

        return Math.min(
            sectionCount - 1,
            Math.round(progress * (sectionCount - 1))
        );
    }

    function createIndicators() {
        if (!nav) return;

        nav.replaceChildren();

        for (let index = 0; index < sectionCount; index++) {
            const button = document.createElement("button");
            button.type = "button";
            button.className = "playr-scroll-step";
            button.setAttribute(
                "aria-label",
                `Scroll to section ${index + 1} of ${sectionCount}`
            );
            button.title = `Section ${index + 1}`;

            const shape = document.createElement("span");
            shape.className = "playr-scroll-shape";
            shape.setAttribute("aria-hidden", "true");

            button.appendChild(shape);

            button.addEventListener("click", () => {
                const maxScroll = getMaxScroll();

                const target =
                    (index / (sectionCount - 1)) * maxScroll;

                window.scrollTo({
                    top: target,
                    behavior: window.matchMedia(
                        "(prefers-reduced-motion: reduce)"
                    ).matches ? "instant" : "smooth"
                });
            });

            nav.appendChild(button);
        }

        activeSection = -1;
        updateActiveIndicator();
    }

    function updateActiveIndicator() {
        if (!nav || sectionCount === 0) return;

        const nextActive = getActiveSection();

        if (nextActive === activeSection) return;

        const buttons = nav.querySelectorAll(".playr-scroll-step");

        buttons.forEach((button, index) => {
            const isActive = index === nextActive;

            button.classList.toggle("is-active", isActive);

            if (isActive) {
                button.setAttribute("aria-current", "location");
            } else {
                button.removeAttribute("aria-current");
            }
        });

        activeSection = nextActive;
    }

    function refresh() {
        scheduled = false;

        const nextNav = document.getElementById("playr-scroll-nav");

        if (!nextNav) {
            nav = null;
            sectionCount = 0;
            activeSection = -1;
            return;
        }

        if (nav !== nextNav) {
            nav = nextNav;
            sectionCount = 0;
            activeSection = -1;
        }

        const nextCount = getSectionCount();

        if (nextCount !== sectionCount) {
            sectionCount = nextCount;
            createIndicators();
        }

        nav.hidden = sectionCount === 0;
        updateActiveIndicator();
    }

    function scheduleRefresh() {
        if (scheduled) return;

        scheduled = true;
        requestAnimationFrame(refresh);
    }

    function observePage() {
        const target = document.querySelector("main");

        if (target === observedElement) {
            scheduleRefresh();
            return;
        }

        resizeObserver?.disconnect();
        observedElement = target;

        if (target && "ResizeObserver" in window) {
            resizeObserver = new ResizeObserver(scheduleRefresh);
            resizeObserver.observe(target);
        }

        scheduleRefresh();
    }

    window.addEventListener("scroll", scheduleRefresh, {
        passive: true
    });

    window.addEventListener("resize", () => {
        observePage();
        scheduleRefresh();
    });

    document.addEventListener("enhancedload", observePage);

    document.addEventListener("DOMContentLoaded", () => {
        observePage();

        setTimeout(observePage, 500);
        setTimeout(observePage, 1500);
    });

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", observePage);
    } else {
        observePage();
    }
})();